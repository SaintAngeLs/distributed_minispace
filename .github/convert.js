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
    let sarif;
    try {
        sarif = JSON.parse(data);
    } catch (parseErr) {
        console.error("Error parsing SARIF file:", parseErr);
        process.exit(1);
    }

    if (!sarif.runs || sarif.runs.length === 0) {
        console.error("No runs found in SARIF file");
        process.exit(1);
    }

    sarif.runs.forEach(run => {
        if (run.tool && run.tool.driver && run.tool.driver.rules) {
            const originalLength = run.tool.driver.rules.length;
            run.tool.driver.rules = run.tool.driver.rules.filter(e => e.id.startsWith("SCS"));
            console.log(`Filtered ${originalLength - run.tool.driver.rules.length} rules out of ${originalLength}`);
        }
    });

    const outputFilePath = path.join(path.dirname(filePath), 'filtered-results.sarif');
    fs.writeFile(outputFilePath, JSON.stringify(sarif), (err) => {
        if (err) {
            console.error("Error writing filtered SARIF file:", err);
            process.exit(1);
        }
        console.log("Filtered SARIF file created at:", outputFilePath);
    });
});
