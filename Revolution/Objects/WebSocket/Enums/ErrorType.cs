using System.ComponentModel;

namespace Revolution.Objects.WebSocket.Enums
{
    internal enum ErrorType : int
    {
        [Description("Uncategorised Error")]
        LabelMe = 0,

        [Description("An Internal Server Error has Occurred")]
        InternalError = 1,

        [Description("The Current Session is invalid")]
        InvalidSession = 2,

        [Description("The Onboarding process has not been finished")]
        OnboardingNotFinished = 3,

        [Description("The current session has already been Authenticated")]
        AlreadyAuthenticated = 4
    }
}
