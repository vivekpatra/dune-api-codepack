# text #
## Summary ##

`text` indicates the text in the active text field, if any.


## Input ##

| **Parameter** | **Value(s)** |
|:--------------|:-------------|
| cmd           | set\_text    |
| text          | {value}      |
| [[timeout](timeout.md)] | {n}          |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="text" value="{value}"/>
  ...
</command_result>
```

## Values ##

`text` can be any UTF-8 encoded text value.

## Notes ##

Text fields may be limited to numeric values. They may also be limited in length.