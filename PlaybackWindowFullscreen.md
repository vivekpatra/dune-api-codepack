# playback\_window\_fullscreen #
## Summary ##

`playback_window_fullscreen`  indicates whether the playback window is sized to fit the screen.

## Input ##

| **Parameter**       | **Value(s)**         |
|:--------------------|:---------------------|
| cmd                 | set\_playback\_state |
| window\_fullscreen  | 1                    |
| [[timeout](timeout.md)]       | {n}                  |

| **Parameter**        | **Value(s)**         |
|:---------------------|:---------------------|
| cmd                  | set\_playback\_state |
| window\_fullscreen   | 0                    |
| window\_rect\_x      | {n}                  |
| window\_rect\_y      | {n}                  |
| window\_rect\_width  | {n}                  |
| window\_rect\_height | {n}                  |
| [[timeout](timeout.md)]        | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_window_fullscreen" value="{value}"/>
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

This parameter was previously named [video\_fullscreen](VideoFullscreen.md) in version 2.