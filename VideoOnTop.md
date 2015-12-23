# video\_on\_top #
## Summary ##

`video_on_top` indicates whether video output is rendered on top of overlay graphics.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| video\_on\_top     | {value}              |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_on_top" value="{value}"/>
  ...
</command_result>
```

## Values ##

|           | **Video On Top** |
|:----------|:-----------------|
| **True**    | `1`              |
| **False**   | `0`              |
| **Default** | `0`              |

## Notes ##

  * This setting takes precedence over the [black\_screen](BlackScreen.md) setting.

  * If you pause the playback and wait for the screensaver, video will still be rendered on top of the screensaver.