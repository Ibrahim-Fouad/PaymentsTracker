using System.ComponentModel;

namespace PaymentsTracker.Common.Enums;

public enum ErrorCode
{
    #region Authentication

    [Description("Invalid email or password.")]
    InvalidEmailOrPassword = 1,

    [Description("Email you have provided is already exists.")]
    EmailIsAlreadyExists,

    #endregion

    [Description("Customer you have requested is not found.")]
    CustomerNotFound,

    [Description("Customer phone you have provided is already registered to another customer.")]
    CustomerPhoneAlreadyExists,
}