using System.ComponentModel;

namespace PaymentsTracker.Common.Enums;

public enum ErrorCode
{
    #region Authentication

    [Description("Invalid email or password.")]
    InvalidEmailOrPassword,

    [Description("Email you have provided is already exists.")]
    EmailIsAlreadyExists,

    #endregion
}