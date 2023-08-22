import { useEffect } from "react";
import { Button, Col, Row, Table, Alert, Form } from "react-bootstrap";
import moment from 'moment';
import { selectState, fetchAsync } from '../slices/matchDetailSlice';
import { useAppDispatch, useAppSelector } from "../app/hooks";
import { useParams } from "react-router-dom";

export default function MatchDetail() {
    let { id } = useParams()
    moment.defaultFormat = 'YYYY-MM-DD HH:mm:ss'
    const state = useAppSelector(selectState)
    const dispatch = useAppDispatch()

    const fetch = () => {
        dispatch(fetchAsync({ id: parseInt(id ?? "") }))
            .unwrap()
            .then(values => {

            })
            .catch(error => {

            })
    }

    useEffect(() => {
        console.log('effect', state)
        fetch()
        return function cleanup() {
            console.log('unmount', state)
        }

    }, [])
    return (
        <>
            <h1>Match Detail</h1>
            {
                state.isLoading &&
                <Alert color='primary'>Loading...</Alert>
            }
            {
                state.error &&
                <Alert show={state.error !== undefined} variant="danger" onClose={() => dispatch({ type: "failure", error: undefined })} dismissible>
                    {state.error}
                </Alert>
            }
            <Row className="my-2">
                <Col className="d-flex justify-content-end">
                    <Button variant="primary" onClick={() => { }}>Add Member</Button><div className="mx-2" />
                    <Button variant="success" onClick={() => { }}>Team Division Rivals</Button>
                </Col>
            </Row>

            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Match</th>
                        <th>Member</th>
                        <th>Is paid</th>
                        <th>Is Skip</th>
                        <th>Modified Date</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        state.data && state.data.map(value =>
                            <tr key={`key-${value.id}`}>
                                <td>{value.id}</td>
                                <td>{value.matchName}</td>
                                <td>{value.memberName}</td>
                                <td><Form.Check disabled checked={value.isPaid} type="switch" /></td>
                                <td><Form.Check disabled checked={value.isSkip} type="switch" /></td>
                                <td>{value.modifiedDate && moment(value.modifiedDate).format()}</td>
                                <td>
                                    <Button variant="warning" className="me-2" onClick={() => { }}>Edit</Button>
                                    <Button variant="danger" className="me-2" onClick={() => { }}>Delete</Button>
                                </td>
                            </tr>
                        )
                    }
                </tbody>
            </Table>
        </>
    )
}