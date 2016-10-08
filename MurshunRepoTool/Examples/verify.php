<?php
$password = "REPO_PASSWORD";
$file = "verify.txt";

if (!empty($_GET['md5']) && !empty($_GET['password'])) {
	if ($_GET['password'] == $password) {
		$data = $_GET['md5'];

		file_put_contents($file, $data);
	};
} else {
	echo file_get_contents($file);
};

?>