# skip\_frames #
## Summary ##

`skip_frames` is used to navigate to the previous or next keyframe.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| skip\_frames  | {value}              |
| [[timeout](timeout.md)] | {n}                  |

## Values ##

|            | **Keyframe** |
|:-----------|:-------------|
| **Previous** | `-1`         |
| **Next**     | `1`          |

## Notes ##

  * Only works if playback speed is 0.
  * Only supported in DVD and MKV playback.
  * You can check the [player\_state](PlayerState.md) parameter to determine whether the player is in `file_playback` or `dvd_playback` mode.
  * In versions 1 and 2, there is no way to tell whether the current file type is MKV.
  * Starting with version 3, you can parse the extension of the `playback_url` parameter.