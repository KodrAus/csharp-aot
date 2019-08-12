param (
    [string]$kind,
    [string]$path
)

curl -i -H "X-KIND:$kind" http://localhost:5000/$path
