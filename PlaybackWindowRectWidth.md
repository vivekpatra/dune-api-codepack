# playback\_window\_rect\_width #
## Summary ##

`playback_window_rect_width` indicates the playback window rectangle's width.

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
  <param name="playback_window_rect_width" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Width**                                  |
|:----------|:-------------------------------------------|
| **Minimum** | `0`                                        |
| **Maximum** | `{osd_width} - {playback_window_rect_x}`   |
| **Default** | `{osd_width}`                              |

## Notes ##

This parameter was previously named [video\_width](VideoWidth.md) in version 2.