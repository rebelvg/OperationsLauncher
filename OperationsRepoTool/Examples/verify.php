<?php
$password = "REPO_PASSWORD";
$file = "verify.txt";

echo @file_get_contents($file);

if (empty($_GET['md5']) || empty($_GET['password']))
    exit();

if ($_GET['password'] !== $password)
    exit();

$data = $_GET['md5'];

@file_put_contents($file, $data);
