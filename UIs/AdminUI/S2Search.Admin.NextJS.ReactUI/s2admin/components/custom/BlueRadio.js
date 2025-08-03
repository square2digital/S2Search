import { withStyles } from "@mui/styles";
import Radio from "@mui/material/Radio";
import { blue } from "@mui/material/colors";

const BlueRadio = withStyles({
  root: {
    color: blue[400],
    "&$checked": {
      color: blue[600],
    },
  },
  checked: {},
})((props) => <Radio color="default" {...props} />);

export default BlueRadio;
