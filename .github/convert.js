const fs = require('fs');
const path = require('path');

const filePath = process.argv[2];

if (!filePath || !fs.existsSync(filePath) || fs.lstatSync(filePath).isDirectory()) {
    console.error("Error: The SARIF file path provided is invalid.");
    process.exit(1);
}

fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
        console.error("Error reading SARIF file:", err);
        process.exit(1);
    }
    let sarif = JSON.parse(data);

    sarif.runs.forEach(run => {
        if (run.tool && run.tool.driver && run.tool.driver.rules) {
            run.tool.driver.rules = run.tool.driver.rules.filter(e => e.id.startsWith("SCS"));
        }
    });

    const outputFilePath = path.join(path.dirname(filePath), 'filtered-results.sarif');
    fs.writeFile(outputFilePath, JSON.stringify(sarif), (err) => {
        if (err) {
            console.error("Error writing filtered SARIF file:", err);
            process.exit(1);
        }
    });
});
