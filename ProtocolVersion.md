# protocol\_version #
## Summary ##

`protocol_version` indicates the installed API version.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="protocol_version" value="{n}"/>
  ...
</command_result>
```

## Values ##

So far, this has always been an integer value (starting at version 1, followed by version 2 and version 3).

## Notes ##

To my dismay, firmware version 130429\_1605\_b6 still carries protocol version 3 even though the API has changed.