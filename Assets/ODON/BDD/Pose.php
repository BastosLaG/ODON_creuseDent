<?php
// Server variables
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "odonbdd";

// User variables (sanitize inputs)
$poseName = $_POST["poseName"];
$poseUser = $_POST["poseUser"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
echo "Connected successfully <br>";

// Use prepared statement to avoid SQL injection
$sql = "SELECT installName FROM dentaldaminstall WHERE installName = ? AND user = ?";

$stmt = $conn->prepare($sql);
$stmt->bind_param("ss", $poseName, $poseUser); // "ss" means two string parameters

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // Output data for each row
    echo "Pose already made";
} else {
    echo "Creating new Pose...";

    // Corrected SQL for inserting new record
    $sql2 = "INSERT INTO dentaldaminstall (installName, user) VALUES (?, ?)";

    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("ss", $poseName, $poseUser); // Bind the parameters for the INSERT query

    if ($stmt2->execute()) {
        echo "New record created successfully";
    } else {
        echo "Error: " . $stmt2->error . "<br>";
    }
}

$stmt->close(); // Close the first statement
$stmt2->close(); // Close the second statement
$conn->close(); // Close the connection
?>
