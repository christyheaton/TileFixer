Usage example:
--------
System reference code is currently set for a REST tile request format of

Examples
--------

This
```
http://{baseUrl}/rest/Spatial/MapTilingService/NamedTiles/{LayerName}/{zIndex}/{xIndex}:{yIndex}/{StaticImageFormatResource}
```
to this
```
http://{baseUrl}/getTile/{LayerName}/{zIndex}/{xIndex}/{yIndex}/{StaticImageFormatResource}
```