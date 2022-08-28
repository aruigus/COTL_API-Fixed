namespace COTL_API.Debug;

public class DebugFollowerCommandClass3 : CustomFollowerCommand.CustomFollowerCommand
{
    public override string InternalName => "DEBUG_FOLLOWER_COMMAND_3";
    
    public override string GetTitle(Follower follower)
    {
        return "Nothing";
    }

    public override string GetLockedDescription(Follower follower)
    {
        return "You shall not pass!";
    }

    public override bool IsAvailable(Follower follower)
    {
        return false;
    }

    public override bool Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand)
    {
        Plugin.Logger.LogDebug("W-wh?! this shouldn't be called!");
        return false;
    }
}