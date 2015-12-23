# playback\_window\_rect\_x #
## Summary ##

`playback_window_rect_x` indicates the playback window rectangle's left margin.

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
  <param name="playback_window_rect_x" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Left Margin**                                |
|:----------|:-----------------------------------------------|
| **Minimum** | `0`                                            |
| **Maximum** | `{osd_width} - {playback_window_rect_width}`   |
| **Default** | `0`                                            |

## Notes ##

This parameter was previously named [video\_x](VideoX.md) in version 2.