namespace Revolution.Internal
{
    internal class WebSocketEventConstants
    {
        public const string Ready = "Ready";
        public const string Pong = "Pong";
        public const string Authenticated = "Authenticated";
        public const string Error = "Error";
        public const string Message = "Message";
        public const string MessageUpdated = "MessageUpdate";
        public const string MessageDeleted = "MessageDelete";
        public const string ChannelCreated = "ChannelCreate";
        public const string ChannelUpdated = "ChannelUpdate";
        public const string ChannelDeleted = "ChannelDelete";
        public const string ChannelGroupJoined = "ChannelGroupJoin";
        public const string ChannelGroupLeft = "ChannelGroupLeave";
        public const string ChannelStartTyping = "ChannelStartTyping";
        public const string ChannelStopTyping = "ChannelStopTyping";
        public const string ChannelAcked = "ChannelAck";
        public const string ServerUpdated = "ServerUpdate";
        public const string ServerDeleted = "ServerDelete";
        public const string ServerMemberUpdated = "ServerMemberUpdate";
        public const string ServerMemberJoined = "ServerMemberJoin";
        public const string ServerMemberLeft = "ServerMemberLeave";
        public const string ServerRoleUpdated = "ServerRoleUpdate";
        public const string ServerRoleDeleted = "ServerRoleDelete";
        public const string UserUpdated = "UserUpdate";
        public const string UserRelationship = "UserRelationship";
    }
}
