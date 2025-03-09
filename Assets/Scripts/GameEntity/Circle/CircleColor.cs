using UnityEngine;

namespace GameEntity.Circle
{
    public enum CircleColorType
    {
        Green,
        Yellow,
        Red
    }
    public static class CircleColor
    {
        public static CircleColorType GetRandomColor()
        {
            int enumLength = System.Enum.GetValues(typeof(CircleColorType)).Length;
            int randomValue = Random.Range(0, enumLength);

            return (CircleColorType)randomValue;
        }

        public static Color GetColorByType(CircleColorType circleColorType)
        {
            switch (circleColorType)
            {
                case CircleColorType.Green:
                    return Color.green;
                case CircleColorType.Yellow:
                    return Color.yellow;
                case CircleColorType.Red:
                    return Color.red;
                default:
                    return Color.white;
            }
        }
    }
}