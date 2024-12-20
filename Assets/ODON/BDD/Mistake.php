<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$mistakeName = $_POST["mistakeName"];
$installStepID = $_POST["installStep"]; // Foreign key referencing the installstep

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully <br>";

// Use prepared statement to avoid SQL injection
$sql = "SELECT mistakeName, mistakeDoneNumber FROM stepmistake WHERE mistakeName = ? AND InstallStep = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("si", $mistakeName, $installStepID); // "si" means string and integer parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Mistake step already exists, increment mistakeDoneNumber
    $row = $result->fetch_assoc();
    $mistakeDoneNumber = $row['mistakeDoneNumber'] + 1;
    echo "Mistake step already exists. Incrementing mistakeDoneNumber to $mistakeDoneNumber.";
} else {
    // Mistake step does not exist, initialize mistakeDoneNumber to 1
    $mistakeDoneNumber = 1;
    echo "Creating new mistake step with mistakeDoneNumber = $mistakeDoneNumber.";
}

// SQL for inserting new record into stepmistake
$sql2 = "INSERT INTO stepmistake (mistakeName, mistakeDoneNumber, InstallStep) VALUES (?, ?, ?)";

$stmt2 = $conn->prepare($sql2);
$stmt2->bind_param("sii", $mistakeName, $mistakeDoneNumber, $installStepID); // Bind the parameters

if ($stmt2->execute()) {
    echo "Success, installID: " . $row["mistakeID"];
} else {
    echo "Error: " . $stmt2->error . "<br>";
}

$stmt->close(); // Close the first statement
$stmt2->close(); // Close the second statement
$conn->close(); // Close the connection
?>
