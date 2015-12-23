# Introduction #

This page contains feature requests for things that are not yet present, or need to be changed. Features are not listed in any particular order; I expect all of these to be added ASAP ;-).


# Feature requests #


---


For crying out loud, start using non-generic XML element names.

I don't want to ever see something like this:
```
<param name="protocol_version" value="3"/>
```

Instead, show me this:
```
<protocol_version>3</protocol_version>
```

This seriously can't be that difficult to implement properly.


---


  * Merge `cmd=get_text` output with `cmd=status`.

Both commands are essentially the same, with the exception that _get\_text_ also gets text from the active text field, if any. Instead, cmd=status should return the text parameter.

If this is impossible (due to technical limitations, not laziness), a new parameter `text_available` should be added to the command results (value: `text_available=1` if text is available; otherwise `text_available=0`). There is currently no way of knowing whether text is available, other than brute forcing the `get_text` command.


---


  * `cmd=sysinfo`

This command should return the following information, formatted in XML:

  1. product ID
  1. firmware version
  1. serial number
  1. uptime (in seconds)


---


  * `cmd=reboot`

Restart the player.


---


  * `cmd=poweroff`

Full power off, regardless of the power button system setting.


---


  * `<param name="black_screen" value="0 | 1 | -1"/>`
  * `<param name="hide_osd" value="0 | 1 | -1"/>`
  * `<param name="action_on_finish" value="exit | restart_playback | -1"/>`

Why weren't these available since version 1? They better be there in version 4!


---


  * `cmd=set_playback_state&subtitles_enabled=0 | 1`
  * `<param name="subtitles_enabled" value="0 | 1 | -1"/>`

There is currently no clean(`*`) mechanism to disable subtitles.

(`*`): setting `subtitles_track` to something >63 disables subtitles, but this is an unacceptable design.


---


  * `<param name="playlist_index" value="{n} | -1"/>`
  * `<param name="playlist.n.playback_url" value="{media_url}"/>`
  * `cmd=set_playback_state&playlist_index={n}`

There needs to be a way to get the media\_url for all entries in a playlist, as well as the current playlist position. There also needs to be a way to change the current position in the playlist without issuing a new `start_playlist_playback` command.