namespace Thuai.Server.GameLogic;

public partial class Battle
{

    #region Fields and properties
    public List<IBullet> Bullets { get; } = [];

    #endregion

    #region Methods

    private bool AddBullet(IBullet bullet)
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error("Cannot add bullet: The battle hasn't started or has ended.");
            return false;
        }
        try
        {
            Bullets.Add(bullet);

            _logger.Debug(
                $"A bullet has been added at ({bullet.BulletPosition.Xpos:F2}, {bullet.BulletPosition.Ypos:F2})"
                + $" with angle {bullet.BulletPosition.Angle:F2}"
            );
            _logger.Verbose("Type: " + bullet.Type.ToString());
            _logger.Verbose("Speed: " + bullet.BulletSpeed);
            _logger.Verbose("Damage: " + bullet.BulletDamage);
            _logger.Verbose("AntiArmor: " + bullet.AntiArmor);

            return true;
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot add bullet: {e.Message}");
            _logger.Debug($"{e}");
            return false;
        }
    }

    private void RemoveBullet(IBullet bullet)
    {
        try
        {
            Bullets.Remove(bullet);

            _logger.Debug($"A bullet at ({bullet.BulletPosition.Xpos:F2}, {bullet.BulletPosition.Ypos:F2}) has been removed.");
        }
        catch (Exception e)
        {
            _logger.Error($"Cannot remove bullet: {e.Message}");
            _logger.Debug($"{e}");
        }
    }

    private double ProjectLength(Position playerPos, Position line)
    {
        double dx = playerPos.Xpos - line.Xpos;
        double dy = playerPos.Ypos - line.Ypos;

        double angleInRadians = (double)(line.Angle * Math.PI / 180.0);
        double lineDirX = (double)Math.Cos(angleInRadians);
        double lineDirY = (double)Math.Sin(angleInRadians);

        double projectionLength = dx * lineDirX + dy * lineDirY;

        return projectionLength;
    }

    private Player? TakeDamage(Position startPos, Position endPos)
    {
        List<Player> tempPlayers = [];
        double min_proj = double.MaxValue;
        Player? finalPlayer = null;
        double line_len = PointDistance(startPos, endPos);
        foreach (Player player in AllPlayers)
        {
            if (LineDistance(startPos, player.PlayerPosition) < Constants.PLAYER_RADIUS)
            {
                tempPlayers.Add(player);
            }
        }
        foreach (Player player in tempPlayers)
        {
            double tempProj = ProjectLength(player.PlayerPosition, startPos);
            if (tempProj > -Constants.PLAYER_RADIUS && tempProj <= line_len)
            {
                if (min_proj > tempProj)
                {
                    min_proj = tempProj;
                    finalPlayer = player;
                }
            }
        }
        return finalPlayer;
    }

    private void UpdateBullets()
    {
        if (Stage != BattleStage.InBattle)
        {
            _logger.Error(
                $"Bullet Cannot be updated when the battle is at stage {Stage}."
            );
            return;
        }

        foreach (IBullet bullet in Bullets)
        {
            try
            {
                double speed = bullet.BulletSpeed;

                foreach (Player player in AllPlayers)
                {
                    if (
                        player.IsAlive == true
                        && player.PlayerArmor.GravityField == true
                        && PointDistance(player.PlayerPosition, bullet.BulletPosition) < Constants.GRAVITY_FIELD_RADIUS
                    )
                    {
                        speed *= Constants.GRAVITY_FIELD_STRENGTH;
                        break;  // Gravity field only affects the bullet once
                    }
                }

                double delta_x = speed * Math.Cos(bullet.BulletPosition.Angle);
                double delta_y = speed * Math.Sin(bullet.BulletPosition.Angle);
                Position endPos = new(delta_x, delta_y);
                Position? finalPos = GetBulletFinalPos(bullet.BulletPosition, endPos, out Position? interPos);
                if (finalPos != null)
                {
                    // TODO: Refactor this part
                    Player? finalPlayer = null;
                    if (interPos != null)
                    {
                        finalPlayer = TakeDamage(bullet.BulletPosition, interPos);
                        if (finalPlayer != null)
                        {
                            finalPlayer.Injured(bullet.BulletDamage, bullet.AntiArmor, out _);

                            // TODO: Implement reflection

                            RemoveBullet(bullet);
                            continue;
                        }
                        finalPlayer = TakeDamage(interPos, finalPos);
                    }
                    else
                    {
                        finalPlayer = TakeDamage(bullet.BulletPosition, finalPos);
                    }
                    if (finalPlayer != null)
                    {
                        finalPlayer.Injured(bullet.BulletDamage, bullet.AntiArmor, out _);

                        // TODO: Implement reflection

                        RemoveBullet(bullet);
                        continue;
                    }

                    bullet.BulletPosition = finalPos;
                }

                _logger.Debug($"Bullets updated.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Bullet Failed to be updated: {ex.Message}");
                _logger.Debug($"{ex}");
            }
        }
    }
    private void Apply_Laser(LaserBullet laserBullet)
    {
        try
        {
            lock (_lock)
            {
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Laser failed to take damage: {ex.Message}");
            _logger.Debug($"{ex}");
        }

    }

    #endregion
}
