result = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"> \
<html> \
	<head> \
		<title></title> \
		<base href="file:///' + WebRootPath + '/" /> \
		<link rel="stylesheet" href="./css/style.css" type="text/css" media="screen" /> \
	</head> \
	<body> \
		<h1>Demo Web page - Action Index2</h1> \
		<a href="#" class="btn-link" onclick="window.external.Action(\'Demo\', \'\'); return false;">Demo1</a> \
		<br /><br /> \
		<a href="#" class="btn-link" onclick="window.external.Action(\'DemoWeb\', \'Index\'); return false;">Web1</a> \
		<br /><br /> \
		<a href="http://google.co.jp/" title="google">google</a> \
		<script src="./js/core.js"></script> \
	</body> \
</html>';