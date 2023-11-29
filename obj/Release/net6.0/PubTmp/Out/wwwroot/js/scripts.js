
window.scrollToBottom = () => {
	const element = document.getElementById("scrollToBottom");
	if (element) {
		element.scrollIntoView({ behavior: "smooth", block: "end" });
	}
};
