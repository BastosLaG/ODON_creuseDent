<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$mistakeName = $_POST["mistakeName"];
$installStepID = $_POST["InstallStep"]; // Foreign key referencing the installstep

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully <br>";

// Use prepared statement to avoid SQL injection
$sql = "SELECT mistakeName, mistakeDoneNumber, mistakeID FROM stepmistake WHERE mistakeName = ? AND InstallStep = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("si", $mistakeName, $installStepID); // "si" means string and integer parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Mistake step already exists, fetch the row and increment mistakeDoneNumber
    $row = $result->fetch_assoc();
    $mistakeDoneNumber = $row['mistakeDoneNumber'] + 1;
    echo "Mistake step already exists. Incrementing mistakeDoneNumber to $mistakeDoneNumber.<br>";

    // Update the existing mistake with the incremented mistakeDoneNumber
    $updateSql = "UPDATE stepmistake SET mistakeDoneNumber = ? WHERE mistakeID = ?";
    $updateStmt = $conn->prepare($updateSql);
    $updateStmt->bind_param("ii", $mistakeDoneNumber, $row['mistakeID']); // "ii" means two integers

    if ($updateStmt->execute()) {
        echo "Mistake step updated successfully. Updated mistakeDoneNumber to: $mistakeDoneNumber.";
    } else {
        echo "Error updating mistake step: " . $updateStmt->error . "<br>";
    }
    
    $updateStmt->close(); // Close the update statement
} else {
    // Mistake step does not exist, initialize mistakeDoneNumber to 1
    $mistakeDoneNumber = 1;
    echo "Creating new mistake step with mistakeDoneNumber = $mistakeDoneNumber.<br>";

    // SQL for inserting new record into stepmistake
    $sql2 = "INSERT INTO stepmistake (mistakeName, mistakeDoneNumber, InstallStep) VALUES (?, ?, ?)";
    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("sii", $mistakeName, $mistakeDoneNumber, $installStepID); // Bind the parameters

    if ($stmt2->execute()) {
        // Inserted a new mistake, retrieve the ID
        $mistakeID = $conn->insert_id;
        echo "New mistake step created. Success, mistakeID: " . $mistakeID;
    } else {
        echo "Error inserting new mistake step: " . $stmt2->error . "<br>";
    }

    $stmt2->close(); // Close the insert statement
}

$stmt->close(); // Close the first select statement
$conn->close(); // Close the connection
?>
