using System;
using System.Data.SqlClient;

namespace FormacaoRazor.Extensions.Helpers
{
    public static class ExceptionHelper
    {
        private static Exception GetInnermostException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        public static bool IsUniqueConstraintViolation(Exception ex)
        {
            if (ex == null) { throw new NullReferenceException(); }
            Exception innermost = GetInnermostException(ex);
            return innermost is SqlException sqlException && sqlException.Class == 14 && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }
    }
}
