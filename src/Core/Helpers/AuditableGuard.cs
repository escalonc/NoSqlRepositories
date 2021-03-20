using System;
using Core.Contracts;

// ReSharper disable once CheckNamespace
namespace Ardalis.GuardClauses
{
    public static class AuditableGuard
    {
        public static void Auditable(this IGuardClause guardClause, IAuditable auditable, string parameterName)
        {
            if (auditable == null)
                throw new ArgumentNullException(parameterName, "Auditable information can't be null");

            if (string.IsNullOrEmpty(auditable.CreatedBy) || auditable.CreatedBy.Trim().Length == 0)
                throw new ArgumentNullException(parameterName, "CreatedBy property can't be null or empty");

            if (string.IsNullOrEmpty(auditable.UpdatedBy) || auditable.CreatedBy.Trim().Length == 0)
                throw new ArgumentNullException(parameterName, "UpdatedBy can't be null or empty");
        }
    }
}