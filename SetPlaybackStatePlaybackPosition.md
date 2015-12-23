# set\_playback\_state: Playback Position #
## Summary ##

Use either one of these parameters, but not both.

## Syntax ##

| **Parameter**                   | **Values**           | **Example**              | **API version(s)** |
|:--------------------------------|:---------------------|:-------------------------|:-------------------|
| [cmd](Cmd.md)                     | set\_playback\_state | cmd=set\_playback\_state | ≥ 1                |
| [[position](PlaybackPosition.md)] | {n}                  | position=0               | ≥ 1                |
| [[skip\_frames](SkipFrames.md)]    | -1 | 1               | skip\_frames=1           | ≥ 1                |
| [[timeout](Timeout.md)]           | {n}                  | timeout=20               | ≥ 1                |

## Examples ##

Set the playback position to 59 minutes:

`http://dune/cgi-bin/do?cmd=set_playback_state&position=3540`


---


Set the playback position to the previous keyframe:

`http://dune/cgi-bin/do?cmd=set_playback_state&speed=0&keyframe=-1`