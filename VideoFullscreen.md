# video\_fullscreen #
## Summary ##

`video_fullscreen` indicates whether the playback window is sized to fit the screen.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| video\_fullscreen  | 1                    |
| [[timeout](timeout.md)]      | {n}                  |

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| video\_fullscreen  | 0                    |
| video\_x           | {n}                  |
| video\_y           | {n}                  |
| video\_width       | {n}                  |
| video\_height      | {n}                  |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_fullscreen" value="{value}"/>
  ...
</command_result>
```

## Values ##

|            | **Full-screen** |
|:-----------|:----------------|
| **True**     | `1`             |
| **False**    | `0`             |
| **Default**  | `1`             |

## Notes ##

This parameter has been renamed to [playback\_window\_fullscreen](PlaybackWindowFullscreen.md) starting with version 3.