import React, { useEffect } from "react";
import { connect } from "react-redux";
import { getUserData } from "../../services/identity/msal";
import { getDashboardSummaryItems } from "../../client/dashboardClient";

// actions
import { setSelectedSearchIndexValue } from "../../redux/actions/selectedSearchIndexActions";
import { setCustomerDetails } from "../../redux/actions/customerActions";
import { setSearchIndexes } from "../../redux/actions/searchIndexesActions";

const BuildReduxData = (props) => {
  useEffect(() => {
    const user = getUserData();
    if (user) {
      const customerId = user.localAccountId;

      setCustomerDetails(customerId);

      setSearchIndexes(customerId);

      getDashboardSummaryItems(customerId).then(function (data) {
        if (data) {
          let arr = [];
          for (let item of [...data.result]) {
            arr.push(
              createData(
                item.searchIndexId,
                item.searchIndexFriendlyName,
                item.notificationsCount,
                item.synonymsCount
              )
            );
          }

          if (arr.length > 0) {
            props.setSelectedSearchIndexValue({
              key: arr[0].searchIndexId,
              value: arr[0].name,
            });
          }
        }
      });
    }
  }, []);

  const createData = (searchIndexId, name, notifications, synonyms) => {
    return { searchIndexId, name, notifications, synonyms };
  };

  return <></>;
};

const mapStateToProps = (reduxState) => {
  return {
    reduxSelectedSearchIndex: reduxState.selectedSearchIndex,
  };
};

const mapDispatchToProps = {
  setSelectedSearchIndexValue,
  setCustomerDetails,
  setSearchIndexes,
};

export default connect(mapStateToProps, mapDispatchToProps)(BuildReduxData);
