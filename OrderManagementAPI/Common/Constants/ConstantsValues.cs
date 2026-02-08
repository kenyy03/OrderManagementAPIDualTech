namespace OrderManagementAPI.Common.Constants
{
    public static class ConstantsValues
    {

        public static class Cliente
        {
            public const int MinLengthNombre = 3;
            public const int MaxLengthNombre = 100;
            public const int LengthIdentidad = 15;
        }

        public static class Producto
        {
            public const int MinLengthNombre = 3;
            public const int MaxLengthNombre = 100;

            public static class Orden
            {
                public const string ErrorAlCrear = "Error al crear la orden";
            }
        }
    }
}
