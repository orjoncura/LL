// Import the necessary modules
import * as fs from 'fs';
import { exec } from 'child_process';
import Constants from '../scripts/Constants'; // Adjust the path as necessary

// Run the AutoRest command
const command: string = `autorest --input-file=${Constants().SwaggerUrl} --typescript --output-folder=src/generated-client`;

exec(command, (error: Error | null, stdout: string, stderr: string) => {
    if (error) {
        console.error(`Error: ${error.message}`);
        return;
    }
    if (stderr) {
        console.error(`Stderr: ${stderr}`);
        return;
    }
    console.log(`Output: ${stdout}`);
});
