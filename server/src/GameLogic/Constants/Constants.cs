namespace Thuai.Server.GameLogic;

/// <summary>
/// Constants used in the game.
/// </summary>
public static partial class Constants
{
    //Armor
    public const int INITIAL_ARMOR_VALUE = 0;
    public const int ARMOR_VALUE_INCREASE = 1;
    public const int INITIAL_HEALTH_VALUE = 3;
    public const int INITIAL_DODGE_PERCENTAGE = 0;      // 0%
    public const int DODGE_PERCENTAGE_INCREASE = 25;    // 25%
    public const float GRAVITY_FIELD_RADIUS = 10f;
    public const float GRAVITY_FIELD_STRENGTH = 0.5f;
    public const int KNIFE_REMAINING_TIME = 10;

    //Weapon
    public const float INITIAL_ATTACK_SPEED = 0.1f;
    public const float ATTACK_SPEED_INCREASE_FACTOR = 2;
    public const float INITIAL_BULLET_SPEED = 2;
    public const float BULLET_SPEED_INCREASE_FACTOR = 2;
    public const int INITIAL_DAMAGE = 1;
    public const int DAMAGE_INCREASE = 1;
    public const int INITIAL_BULLETS = 5;
    public const int BULLETS_INCREASE = 1;
    public const int ANTI_ARMOR_FACTOR = 2;

    //Player
    public const float MOVE_SPEED = 0.1f;
    public const float TURN_SPEED = (float)Math.PI / 60;
    public const float PLAYER_RADIUS = 0.5f;
    public const float BULLET_GENERATE_DISTANCE = 0.8f;

    // Bullet
    public const float BULLET_RADIUS = 0.1f;
    public const int BULLET_REMAINING_TICKS = 100;
    public const float LASER_WIDTH = 0.1f;
    public const float LASER_LENGTH_EQUAVALENT_BULLET_FLYING_TICKS = 20;

    //Map
    public const float WALL_LENGTH = 10.0f;
    public const float WALL_THICK = 0.1f;

    // Wall
    public const int BREAKABLE_WALL_MAX_COLLIDE_COUNT = 3;
}
