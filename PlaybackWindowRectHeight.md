# playback\_window\_rect\_height #
## Summary ##

`playback_window_rect_height` indicates the playback window rectangle's height.

## Input ##

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
  <param name="playback_window_rect_height" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Height**                                  |
|:----------|:--------------------------------------------|
| **Minimum** | `0`                                         |
| **Maximum** | `{osd_height} - {playback_window_rect_y}`   |
| **Default** | `{osd_height}`                              |

## Notes ##

This parameter was previously named [video\_height](VideoHeight.md) in version 2.