PROTOC=tools/Grpc.Tools.1.22.0/linux_x64/protoc
PLUGIN=tools/Grpc.Tools.1.22.0/linux_x64/grpc_csharp_plugin
PROTO=Operations.proto
SERVER=src/Pacco.Services.Operations.Api
CLIENT=src/Pacco.Services.Operations.GrpcClient

$PROTOC --csharp_out $SERVER --grpc_out $SERVER --plugin=protoc-gen-grpc=$PLUGIN $SERVER/$PROTO
$PROTOC --csharp_out $CLIENT --grpc_out $CLIENT --plugin=protoc-gen-grpc=$PLUGIN $CLIENT/$PROTO