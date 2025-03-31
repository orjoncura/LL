sudo apt install python3 python3-pip -y

sudo apt install python3-venv -y
python3 -m venv my_tts_env

source my_tts_env/bin/activate
pip install TTS
tts --text "Hello, this is Coqui TTS!" --model_name tts_models/en/ljspeech/tacotron2-DDC --out_path output.wav

--libretranslate
docker pull libretranslate/libretranslate
docker run -d -p 5000:5000 libretranslate/libretranslate --load-only en,es