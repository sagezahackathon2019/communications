$(function () {

	$("#register").on("click", function () {

		var endpoint = "api/vendors/register";

		var vendorName = $("#vendorName").val();
		var vendorEmail = $("#vendorEmail").val();

		var vendor = {
			DefaultFromAddress: vendorEmail,
			Name: vendorName
		};

		var payload = JSON.stringify(vendor);

		$.ajax({
			type: "POST",
			url: endpoint,
			data: payload,
			success: function () {

			},
			contentType: "application/json"
		})
		.done(function () {
			console.log("success");
		})
		.fail(function () {
			console.log("error");
		});
	
			

	});

});