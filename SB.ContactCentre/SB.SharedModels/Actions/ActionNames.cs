namespace SB.SharedModels.Actions
{
    public class ActionNames
    {
        public const string ActionTracking = "sb_ActionTracking";
        public class ActionTrackingNames
        {
            public const string CreatePhoneCall = nameof(CreatePhoneCall);
            public const string ResettingCallCounterOnLine = nameof(ResettingCallCounterOnLine);
            public const string CurrentDateTime = nameof(CurrentDateTime);
            public const string CreateTask = nameof(CreateTask);
            public const string GetPhoneNumbersForUnclosedCards = nameof(GetPhoneNumbersForUnclosedCards);
            public const string ClosePhoneCall = nameof(ClosePhoneCall);
            public const string CloseOldPhoneCalls = nameof(CloseOldPhoneCalls);
        }
    }
}