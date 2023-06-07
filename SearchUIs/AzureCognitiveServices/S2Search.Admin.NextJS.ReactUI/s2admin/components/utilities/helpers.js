export const homePageRedirectOnNullSearchIndex = (selectedSearchIndex) => {
  if (!selectedSearchIndex) {
    window.location.replace("/");
  }
};
