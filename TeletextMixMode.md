# teletext\_mix\_mode #
## Summary ##

`teletext_mix_mode` indicates whether to enable teletext mix mode.

## Input ##

| **Parameter**       | **Value(s)**         |
|:--------------------|:---------------------|
| cmd                 | set\_playback\_state |
| teletext\_mix\_mode | {value}              |
| [[timeout](timeout.md)]       | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="teletext_mix_mode" value="{value}"/>
  ...
</command_result>
```

## Values ##

|            | **Mix Mode** |
|:-----------|:-------------|
| **Enabled**  | `1`          |
| **Disabled** | `0`          |