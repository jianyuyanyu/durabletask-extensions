import {
  Box,
  Breadcrumbs,
  Button,
  Grid,
  IconButton,
  Link,
  Paper,
  Typography,
} from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { useSnackbar } from "notistack";
import React, { useState } from "react";
import { Link as RouterLink, useHistory } from "react-router-dom";
import * as yup from "yup";
import { apiAxios } from "../../apiAxios";
import { ErrorAlert } from "../../components/ErrorAlert";
import { TextField } from "../../form/TextField";
import { useForm } from "../../form/useForm";
import {
  CreateOrchestrationRequest,
  OrchestrationInstance,
} from "../../models/ApiModels";

const schema = yup
  .object({
    name: yup.string().label("Name").required(),
    version: yup.string().label("Version"),
    instanceId: yup.string().label("Instance Id"),
    input: yup.string().label("Input").json(),
    tags: yup
      .array(
        yup
          .object({
            key: yup.string().label("Key").required(),
            value: yup.string().label("Value").required(),
          })
          .required()
      )
      .defined(),
  })
  .required();

export function Create() {
  const form = useForm(schema, () => ({
    name: "",
    version: "",
    instanceId: "",
    input: "",
    tags: [],
  }));
  const [error, setError] = useState<any>();

  const history = useHistory();
  const { enqueueSnackbar } = useSnackbar();

  async function handleSaveClick() {
    try {
      setError(undefined);

      const request: CreateOrchestrationRequest = {
        name: form.value.name,
        version: form.value.version,
        instanceId: form.value.instanceId,
        input: form.value.input ? JSON.parse(form.value.input) : null,
        tags: form.value.tags.reduce((previous, current) => {
          previous[current.key] = current.value;
          return previous;
        }, {} as Record<string, string>),
      };

      var response = await apiAxios.post<OrchestrationInstance>(
        `/v1/orchestrations`,
        request
      );

      enqueueSnackbar("Orchestration created", {
        variant: "success",
      });

      history.push(
        `/orchestrations/${encodeURIComponent(response.data.instanceId)}`
      );
    } catch (error) {
      setError(error);
    }
  }

  return (
    <div>
      <Box marginBottom={2}>
        <Breadcrumbs aria-label="breadcrumb">
          <Link component={RouterLink} to="/orchestrations">
            Orchestrations
          </Link>
          <Typography color="textPrimary">Create</Typography>
        </Breadcrumbs>
      </Box>
      <Paper variant="outlined">
        <Box padding={2}>
          <Grid container spacing={2}>
            <Grid item xs={6}>
              <TextField field={form.field("name")} />
            </Grid>
            <Grid item xs={6}>
              <TextField field={form.field("version")} />
            </Grid>
            <Grid item xs={6}>
              <TextField field={form.field("instanceId")} />
            </Grid>
            <Grid item xs={6}></Grid>
            <Grid item xs={12}>
              <TextField field={form.field("input")} multiline rows={6} />
            </Grid>
            <Grid item xs={12}>
              <Button
                onClick={() => form.field("tags").push({ key: "", value: "" })}
              >
                Add tag
              </Button>
            </Grid>
            {form.field("tags").render((field) =>
              field.fields().map((tagField) => (
                <Grid key={tagField.path} item xs={12}>
                  <Box display="flex">
                    <Box marginX={1} flex={1}>
                      <TextField field={tagField.field("key")} />
                    </Box>
                    <Box marginX={1} flex={1}>
                      <TextField field={tagField.field("value")} />
                    </Box>
                    <Box>
                      <IconButton onClick={() => field.remove(tagField.value)}>
                        <DeleteIcon />
                      </IconButton>
                    </Box>
                  </Box>
                </Grid>
              ))
            )}
            {error && (
              <Grid item xs={12}>
                <ErrorAlert error={error} />
              </Grid>
            )}
            {form.render((form) => (
              <Grid item xs={12} container spacing={1} justify="space-between">
                <Grid item>
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={handleSaveClick}
                    disabled={
                      form.pendingValidation ||
                      Object.keys(form.errors).length > 0
                    }
                  >
                    Create
                  </Button>
                </Grid>
                <Grid item>
                  <Button onClick={() => form.reset()}>Reset</Button>
                </Grid>
              </Grid>
            ))}
          </Grid>
        </Box>
      </Paper>
    </div>
  );
}
