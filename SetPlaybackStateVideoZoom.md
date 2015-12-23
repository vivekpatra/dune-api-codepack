# set\_playback\_state: Video Zoom #
## Summary ##

The following parameters are used to set a custom video zoom setting.

To apply a custom zoom setting, you must specify all five zoom parameters. Set the video/window fullscreen parameter to 0, and fill in proper values for the other parameters.

To revert to the default zoom setting, you must only specify video/window fullscreen parameter with a value of 1.

The parameter names used depend on the protocol version. Parameters for version 3 cannot be used in version 2, and vice versa.

## Syntax: Version 2 ##

| **Parameter**                        | **Value(s)**         | **Example**              | **API version(s)** |
|:-------------------------------------|:---------------------|:-------------------------|:-------------------|
| [cmd](Cmd.md)                          | set\_playback\_state | cmd=set\_playback\_state | ≥ 1                |
| [video\_fullscreen](VideoFullscreen.md) | 0 | 1                | video\_fullscreen=0      | 2                  |
| [video\_x](VideoX.md)                   | {n}                  | video\_x=192             | 2                  |
| [video\_y](VideoY.md)                   | {n}                  | video\_y=108             | 2                  |
| [video\_width](VideoWidth.md)           | {n}                  | video\_width=1536        | 2                  |
| [video\_height](VideoHeight.md)         | {n}                  | video\_height=864        | 2                  |
| [[timeout](Timeout.md)]                | {n}                  | timeout=20               | ≥ 1                |

## Examples: Version 2 ##

Downscale 1080p (1920×1080) output to 720p (1280×720), but keep it centered on the display:

`http://dune/cgi-bin/do?cmd=set_playback_state&video_fullscreen=0&video_x=320&video_y=160&video_width=1280&video_height=720`


---


Revert back to 1080p (standard output):

`http://dune/cgi-bin/do?cmd=set_playback_state&video_fullscreen=1`


---


## Syntax: Version ≥ 3 ##

| **Parameter**                                   | **Value(s)**         | **Example**              | **API version(s)** |
|:------------------------------------------------|:---------------------|:-------------------------|:-------------------|
| [cmd](Cmd.md)                                     | set\_playback\_state | cmd=set\_playback\_state | ≥ 1                |
| [window\_fullscreen](PlaybackWindowFullscreen.md)  | 0 | 1                | window\_fullscreen=0     | ≥ 3                |
| [window\_rect\_x](PlaybackWindowRectX.md)           | {n}                  | window\_rect\_x=192      | ≥ 3                |
| [window\_rect\_y](PlaybackWindowRectY.md)           | {n}                  | window\_rect\_y=108      | ≥ 3                |
| [window\_rect\_width](PlaybackWindowRectWidth.md)   | {n}                  | window\_rect\_width=1536 | ≥ 3                |
| [window\_rect\_height](PlaybackWindowRectHeight.md) | {n}                  | window\_rect\_height=864 | ≥ 3                |
| [[timeout](Timeout.md)]                           | {n}                  | timeout=20               | ≥ 1                |

## Examples: Version ≥ 3 ##

Downscale 1080p (1920×1080) output to 720p (1280×720), but keep it centered on the display:

`http://dune/cgi-bin/do?cmd=set_playback_state&window_fullscreen=0&window_rect_x=320&window_rect_y=160&window_rect_width=1280&window_rect_height=720`


---


Revert back to 1080p (standard output):

`http://dune/cgi-bin/do?cmd=set_playback_state&window_fullscreen=1`