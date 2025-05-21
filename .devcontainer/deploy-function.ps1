dotnet publish /workspace/src/Function/ -c release;

cd /workspace/src/Function/bin/release/net9.0/publish/;

zip -r function.zip .;

az webapp deploy -g group -n function --src-path ./function.zip --type zip;

cd /workspace/