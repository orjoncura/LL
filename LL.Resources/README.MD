--Database auditing

There are two ways of database auditing
the tables that are expected to get updated will have a shadow table
we will insert a record to the shodow table each time the main table is updated.
The tables that are not expected to get updated, will have only a 'CreatedBy'
column and their values shouldn't change.

--Add migration

Package Mangager Console
Add-Migration YourMigrationName' --project LL.Data  --startup-project LL.API'
Update-Database' --project LL.Data  --startup-project LL.API'

Command Line
Linux: export DOTNET_ROOT=/user/bit/dotnet
export PATH="$PATH:/home/orjoncura/.dotnet/tools"
Windows: dotnet ef migrations add Intial --project LL.Data  --startup-project LL.API
dotnet ef database update --project LL.Data  --startup-project LL.API

--Text to Speech

sudo apt install python3 python3-pip -y

sudo apt install python3-venv -y
python3 -m venv my_tts_env

source my_tts_env/bin/activate
pip install TTS
tts --text "Hello, this is Coqui TTS!" --model_name tts_models/en/ljspeech/tacotron2-DDC --out_path output.wav
