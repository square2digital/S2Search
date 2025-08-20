export const LogDetails = props => {
  if (process.env.NODE_ENV !== 'production' && props.enable) {
    logProps(props);
  }

  return null;
};

const logProps = props => {
  if (props.logData) {
    for (const key in props.logData) {
      console.dir(
        `${key} - ${
          props.logData[key] ? JSON.stringify(props.logData[key]) : 'empty'
        }`
      );
    }
  }
};
