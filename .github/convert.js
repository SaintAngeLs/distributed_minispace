const fs = require('fs');

fs.readFile('results.sarif', 'utf8', (err, data) => {
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

    fs.writeFile('filtered-results.sarif', JSON.stringify(sarif), (err) => {
        if (err) {
            console.error("Error writing filtered SARIF file:", err);
            process.exit(1);
        }
    });
});
