# Command Results #
## Summary ##
Command results are formatted as a simple XML document.

Even though the output format is the same across all versions of the API, personally I have very strong feelings against most of the design choices that were made.


---


## Syntax ##
The XML document has one root node: ` <command_result> ... </command_result> `

This node is populated with exactly one child element `<param />` for every command result parameter.

Each element is then given a "`name`" and "`value`" attribute.

```
<command_result>
	<param name="{parameter_m}" value="{value}"/>
	...
	<param name="{parameter_n}" value="{value}"/>
</command_result>
```

Use the [status](StatusCommand.md) command to retrieve a list of command results representing the current player state.


---


## Values ##

Below is a list of all possible command result parameters. Some parameters are always returned, others are specific to a certain player/playback state and/or command. A parameter value of -1 means "not applicable". While it may seem to indicate "disabled", this is not the case. You can't force a value to -1 either, so there is no mechanism to disable parameters.

| **Parameter**| **Type**| **Summary**| **API Version(s)**|
|:-------------|:--------|:-----------|:------------------|
|[command\_status](CommandStatus.md)|String   |Indicates whether the command was successful.|≥ 1                |
|[error\_kind](ErrorKind.md)|String   |Indicates the error kind when _command\_status_ is 'failed'.|≥ 1                |
|[error\_description](ErrorDescription.md)|String   |The error description.|≥ 1                |
|[protocol\_version](ProtocolVersion.md)|Integer  |Indicates the API version.|≥ 1                |
|[player\_state](PlayerState.md)|String   |Indicates the player state.|≥ 1                |
|[playback\_speed](PlaybackSpeed.md)|Integer  |Indicates the playback speed.|≥ 1                |
|[playback\_duration](PlaybackDuration.md)|Integer  |Indicates the playback duration in seconds.|≥ 1                |
|[playback\_position](PlaybackPosition.md)|Integer  |Indicates playback position in seconds.|≥ 1                |
|[playback\_is\_buffering](PlaybackIsBuffering.md)|Boolean  |Indicates whether the playback is buffering.|≥ 1                |
|[playback\_volume](PlaybackVolume.md)|Integer  |Indicates the playback volume percentage.|≥ 2                |
|[playback\_mute](PlaybackMute.md)|Boolean  |Indicates whether the playback volume is muted.|≥ 2                |
|[audio\_track](AudioTrack.md)|Integer  |Indicates the active audio track (i.e. the language track) of the current playback.|≥ 2                |
|[video\_fullscreen](VideoFullscreen.md)|Boolean  |Indicates whether custom playback window zoom settings are applied.|2                  |
|[video\_x](VideoX.md)|Integer  |Indicates the playback window rectangle's left margin.|2                  |
|[video\_y](VideoY.md)|Integer  |Indicates the playback window rectangle's top margin.|2                  |
|[video\_width](VideoWidth.md)|Integer  |Indicates the playback window rectangle's width.|2                  |
|[video\_height](VideoHeight.md)|Integer  |Indicates the playback window rectangle's height.|2                  |
|[playback\_window\_fullscreen](PlaybackWindowFullscreen.md)|Boolean  |Same as _video\_fullscreen_, it was renamed in version 3.|≥ 3                |
|[playback\_window\_rect\_x](PlaybackWindowRectX.md)|Integer  |Same as _video\_x_, it was renamed in version 3.|≥ 3                |
|[playback\_window\_rect\_y](PlaybackWindowRectY.md)|Integer  |Same as _video\_y_, it was renamed in version 3.|≥ 3                |
|[playback\_window\_rect\_width](PlaybackWindowRectWidth.md)|Integer  |Same as _video\_width_, it was renamed in version 3.|≥ 3                |
|[playback\_window\_rect\_height](PlaybackWindowRectHeight.md)|Integer  |Same as _video\_height_, it was renamed in version 3.|≥ 3                |
|[video\_total\_display\_width](VideoTotalDisplayWidth.md)|Integer  |The screen's total display width.|2                  |
|[video\_total\_display\_height](VideoTotalDisplayHeight.md)|Integer  |The screen's total display height.|2                  |
|[osd\_width](OsdWidth.md)|Integer  |Same as _video\_total\_display\_width_, it was renamed in version 3.|≥ 3                |
|[osd\_height](OsdHeight.md)|Integer  |Same as _video\_total\_display\_height_, it was renamed in version 3.|≥ 3                |
|[playback\_video\_width](PlaybackVideoWidth.md)|Integer  |Indicates the video stream's width.|≥ 3                |
|[playback\_video\_height](PlaybackVideoHeight.md)|Integer  |Indicates the video stream's height.|≥ 3                |
|[video\_enabled](VideoEnabled.md)|Boolean  |Indicates whether video output is enabled.|≥ 2                |
|[video\_zoom](VideoZoom.md)|String   |Indicates the video zoom mode.|≥ 2                |
|[playback\_dvd\_menu](PlaybackDvdMenu.md)|Boolean  |Indicates whether a DVD menu is currently visible.|≥ 1                |
|[playback\_bluray\_dmenu](PlaybackBlurayMenu.md)|Boolean  |Indicates whether a bluray menu is currently visible.|≥ 1                |
|[playback\_state](PlaybackState.md)|String   |Indicates the playback state.|≥ 3                |
|[previous\_playback\_state](PreviousPlaybackState.md)|String   |Indicates the previous playback state.|≥ 3                |
|[last\_playback\_event](LastPlaybackEvent.md)|String   |Indicates the last playback event.|≥ 3                |
|[playback\_url](PlaybackUrl.md)|String   |Indicates the media URL that is currently playing.|≥ 3                |
|[subtitles\_track](SubtitlesTrack.md)|Integer  |Indicates the active subtitle track (i.e. the language track) of the current playback.|≥ 3                |
|[playback\_clip\_rect\_x](PlaybackClipRectX.md)|Integer  |Indicates the clipping region's left margin.|≥ 3                |
|[playback\_clip\_rect\_y](PlaybackClipRectY.md)|Integer  |Indicates the clipping region's top margin.|≥ 3                |
|[playback\_clip\_rect\_width](PlaybackClipRectWidth.md)|Integer  |Indicates the clipping region's width.|≥ 3                |
|[playback\_clip\_rect\_height](PlaybackClipRectHeight.md)|Integer  |Indicates the clipping region's height.|≥ 3                |
|[video\_on\_top](VideoOnTop.md)|Boolean  |Indicates whether the video is shown on top.|≥ 3                |
|[audio\_track.n.lang](AudioTrackLang.md)|String   |Indicates the language of audio track number _n_.|≥ 2                |
|[audio\_track.n.codec](AudioTrackCodec.md)|String   |Indicates the codec of audio track number _n_.|≥ 3                |
|[subtitles\_track.n.lang](SubtitlesTrackLang.md)|String   |Indicates the language of subtitles track number _n_.|≥ 3                |
|[subtitles\_track.n.codec](SubtitlesTrackCodec.md)|String   |Indicates the format of subtitles track number _n_.|≥ 3                |
|[text](Text.md)|String   |Indicates the text in the selected text field.|≥ 3                |
|[pause\_is\_available](PauseIsAvailable.md) |Boolean  |Indicates whether playback can be paused.| ?                 |
|[teletext\_available](TeletextAvailable.md) |Boolean  |Indicates whether teletext is available.| ?                 |
|[teletext\_enabled](TeletextEnabled.md) |Boolean  |Indicates whether teletext is enabled.| ?                 |
|[teletext\_mix\_mode](TeletextMixMode.md) |Boolean  |Indicates whether teletext mix mode is enabled. | ?                 |
|[teletext\_page\_number](TeletextPageNumber.md) |Boolean  |Indicates the current teletext page.| ?                 |