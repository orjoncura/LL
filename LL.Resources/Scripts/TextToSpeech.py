import subprocess
import os
import sys

def synthesize_audio(text, model_name, output_path):
    # Define the virtual environment path and tts executable path
    virtual_env_path = "/home/orjoncura/my_tts_env"  # Path to your virtual environment
    tts_command_path = f"{virtual_env_path}/bin/tts"  # Path to the TTS executable

    # Check if the TTS executable exists
    if not os.path.exists(tts_command_path):
        print(f"Error: tts command not found at {tts_command_path}")
        sys.exit(1)

    try:
        # Construct the TTS command
        command = [
            tts_command_path,
            "--text", text,
            "--model_name", model_name,
            "--out_path", output_path
        ]

        # Run the command
        print("Running TTS synthesis...")
        result = subprocess.run(command, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)

        # Check for errors
        if result.returncode == 0:
            print(f"Audio file successfully generated at: {output_path}")
        else:
            print("Error during synthesis:")
            print(result.stderr)

    except Exception as e:
        print(f"An error occurred: {e}")
        sys.exit(1)

if __name__ == "__main__":
    # Expecting 3 arguments: text, model_name, and output_path
    if len(sys.argv) != 4:
        print("Usage: python synthesize_audio.py '<text>' <model_name> <output_path>")
        sys.exit(1)

    text = sys.argv[1]
    model_name = sys.argv[2]
    output_path = sys.argv[3]

    synthesize_audio(text, model_name, output_path)
