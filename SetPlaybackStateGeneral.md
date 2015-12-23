# set\_playback\_state: General #

## Summary ##

You can mix and match these parameters as you wish, so long as the values are valid.

## Syntax ##

| **Parameter**                         | **Values**           | **Example**                  | **API version(s)**|
|:--------------------------------------|:---------------------|:-----------------------------|:------------------|
| [cmd](Cmd.md)                           | set\_playback\_state | cmd=set\_playback\_state     | ≥ 1               |
| [[speed](PlaybackSpeed.md)]             | {n}                  | speed=256                    | ≥ 1               |
| [[black\_screen](BlackScreen.md)]        | 0 | 1                | black\_screen=0              | ≥ 1               |
| [[hide\_osd](HideOsd.md)]                | 0 | 1                | hide\_osd=0                  | ≥ 1               |
| [[action\_on\_finish](ActionOnFinish.md)] | {action}             | action\_on\_finish=exit      | ≥ 1               |
| [[action\_on\_exit](ActionOnExit.md)]     | {action}             | action\_on\_exit=main\_screen | N/A (`*`)         |
| [[volume](PlaybackVolume.md)]           | 0 .. 100             | volume=100                   | ≥ 2               |
| [[mute](PlaybackMute.md)]               | 0 | 1                | mute=0                       | ≥ 2               |
| [[audio\_track](AudioTrack.md)]          | 0 .. {n}             | audio\_track=0               | ≥ 2               |
| [[subtitles\_track](SubtitlesTrack.md)]  | 0 .. {n}             | subtitles\_track=0           | ≥ 3               |
| [[video\_enabled](VideoEnabled.md)]      | 0 | 1                | video\_enabled=1             | ≥ 2               |
| [[video\_zoom](VideoZoom.md)]            | {zoom}               | video\_zoom=normal           | ≥ 2               |
| [[video\_on\_top](VideoOnTop.md)]         | 0 | 1                | video\_on\_top=0             | ≥ 3               |
| [[timeout](Timeout.md)]                 | {n}                  | timeout=20                   | ≥ 1               |

(`*`): The action\_on\_exit parameter was discovered accidentally. It is recognized as a valid parameter, but it currently doesn't work (yet).

## Examples ##

Stretch 4:3 video to fit a 16:9 display:

`http://dune/cgi-bin/do?cmd=set_playback_state&zoom=full_stretch`


---


Set the volume to 70%:

`http://dune/cgi-bin/do?cmd=set_playback_state&volume=70`


---


Mute the volume and disable the video output:

`http://dune/cgi-bin/do?cmd=set_playback_state&mute=1&black_screen=1`


---


Set the playback speed to fast forward (2x), turn on repeat:

`http://dune/cgi-bin/do?cmd=set_playback_state&speed=512&action_on_finish=restart_playback`