namespace OrderManagementAPI.Common.Constants
{
    public static class ErrorMessages
    {
        public const string InternalServerError = "Internal Server Error";
        public const string RequestCannotBeNull = "Request no puede ser nulo";
        public const string EnsureBodyIsSent = "Asegurarse de que el cuerpo de la peticion se este enviando.";
        
        public static class Cliente
        {
            public const string ErrorAlCrear = "Error al crear el cliente";
            public const string ErrorAlObtener = "Error al obtener el cliente";
            public const string ErrorAlActualizar = "Error al actualizar el cliente";
            public const string ErrorAlEliminar = "Error al eliminar el cliente";
            public const string NoEncontrado = "Cliente no encontrado";
            public const string IdNoEnviar = "Asegurarse de no enviar el ClienteId, este se genera automaticamente.";
            public const string IdMayorCero = "El ID del cliente debe ser mayor a 0.";
            public const string NombreLongitud = "El nombre del cliente debe tener entre 3 y 100 caracteres.";
            public const string IdentidadLongitud = "La identidad del cliente debe tener 15 caracteres considerando guiones.";
            public const string IdentidadDuplicada = "Ya existe un cliente con la misma identidad {0}";
            public const string ClienteNoEncontradoConId = "No se encontró un cliente con el ID {0}.";
        }

        public static class Producto
        {
            public const string ErrorAlCrear = "Error al crear el producto";
            public const string ErrorAlObtener = "Error al obtener el producto";
            public const string ErrorAlActualizar = "Error al actualizar el producto";
            public const string ErrorAlEliminar = "Error al eliminar el producto";
            public const string NoEncontrado = "Producto no encontrado";
            public const string IdMayorCero = "El ID del producto debe ser mayor a 0.";
            public const string ProductoNoEncontradoConId = "No se encontró un producto con el ID {0}.";
            public const string IdNoEnviar = "Asegurarse de no enviar el ProductoId, este se genera automaticamente.";
            public const string NombreLongitud = "El nombre del cliente debe tener entre 3 y 100 caracteres.";
            public const string PrecioMayorCero = "El precio debe ser mayor a cero.";
            public const string ExistenciaMayorIgualCero = "La existencia debe ser mayor o igual a cero.";
            public const string ProductoSinExistencia = "Producto {0} no tiene suficiente existencia. Existencia actual: {1}, cantidad solicitada: {2}.";
        }

        public static class Orden
        {
            public const string ErrorAlCrear = "Error al crear la orden";
            public const string ErrorAlObtener = "Error al obtener la orden";
            public const string ErrorAlActualizar = "Error al actualizar la orden";
            public const string ErrorAlEliminar = "Error al eliminar la orden";
            public const string NoEncontrado = "Orden no encontrada";
            public const string IdMayorCero = "El ID de la orden debe ser mayor a 0.";
            public const string OrdenNoEncontradaConId = "No se encontró una orden con el ID {0}.";
        }

        public static string FormatException(Exception ex)
        {
            return $"ErrorMessage: {ex.Message}.\nStackTrace: {ex.StackTrace}";
        }
    }

    public static class SuccessMessages
    {
        public static class Cliente
        {
            public const string CreadoExitosamente = "Cliente creado exitosamente";
            public const string ActualizadoExitosamente = "Cliente actualizado exitosamente";
            public const string EliminadoExitosamente = "Cliente eliminado exitosamente";
        }

        public static class Producto
        {
            public const string CreadoExitosamente = "Producto creado exitosamente";
            public const string ActualizadoExitosamente = "Producto actualizado exitosamente";
            public const string EliminadoExitosamente = "Producto eliminado exitosamente";
        }

        public static class Orden
        {
            public const string CreadaExitosamente = "Orden creada exitosamente";
            public const string ActualizadaExitosamente = "Orden actualizada exitosamente";
            public const string EliminadaExitosamente = "Orden eliminada exitosamente";
        }
    }
}
