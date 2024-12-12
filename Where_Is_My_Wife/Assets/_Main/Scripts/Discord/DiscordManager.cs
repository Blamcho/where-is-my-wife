using System;
using UnityEngine;

public class DiscordManager : Singleton<DiscordManager>
{
    private Discord.Discord _discord;
    
    private void Start()
    {
        _discord = new Discord.Discord(1316564719301165077, (ulong) Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    private void OnDisable()
    {
        _discord.Dispose();
    }

    private void Update()
    {
        _discord.RunCallbacks();
    }

    private void ChangeActivity()
    {
        var activityManager = _discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Playing",
            Details = "Get it for free on itch.io!",
            Assets =
            {
                LargeImage = "logo",
            },
            Timestamps = 
            {
                Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            },
        };
        activityManager.UpdateActivity(activity, result =>
        {
            if (result != Discord.Result.Ok)
            {
                Debug.LogError("Failed to update activity");
            }
        });
    }
}
